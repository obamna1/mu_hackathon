# Developer Guide

## Overview
This project implements a music NFT minting system using a .NET MVC backend and a Node.js script for Solana blockchain interaction. The core logic is split between:
- **Backend**: C# controllers and EF Core models for data access and orchestration.
- **Minting Script**: Node.js module (`mint_js/mint.js`) that performs on-chain NFT minting.

---

## 1. Minting Flow

High-level steps:

1. **Metadata Storage**  
   Song/NFT metadata is stored in the SQL database table `SongNFTMetadata`.

2. **Mint Request**  
   A client issues an HTTP GET or POST to `/Mint?id={songId}`.

3. **Metadata Fetch & Serialization**  
   `MintController.Index(int id)`:
   - Looks up the record by `id` via `SongMetaDbContext`.
   - Builds a metadata JSON object (title, ISRC, writers, etc.).
   - Writes it to a temporary JSON file.

4. **Invoke Minting Script**  
   - Spawns a Node.js process to run `mint_js/mint.js`, passing the temp JSON path.
   - Captures stdout and stderr from the script.

5. **Return Result**  
   - On success or failure, the combined log (stdout + stderr) is returned as the HTTP response content.

---

## 2. Backend Modules

### 2.1 MintController.cs
- **Route**: `[Route("Mint")]` (handles GET and POST at `/Mint`)
- **Dependencies**:
  - `SongMetaDbContext` for DB access.
  - `IHttpClientFactory` (unused in current mint flow but available).
- **Key logic**:
  1. Fetch `SongNFTMetadata` by `id`.
  2. Build and serialize metadata JSON.
  3. Write JSON to `mint_temp.json`.
  4. Execute the Node.js mint script via `ProcessStartInfo`.
  5. Aggregate and return logs.

### 2.2 SongNFTMetadata.cs
- **Purpose**: EF Core entity mapped to the `SongNFTMetadata` table.
- **Fields**:
  - `Id` (int)
  - `Title`, `Isrc`
  - `Writer1`, `Writer2`
  - `Publisher1`, `Publisher2`
  - `AscapShare` (int)
  - `Artist`, `ReleaseDate`, `Copyright`
  - `DurationSeconds`, `Explicit` (bool)
  - `Language`, `Distributor`, `OriginCountry`

### 2.3 SongMetaDbContext.cs
- **Purpose**: EF Core `DbContext` exposing `DbSet<SongNFTMetadata>`.
- **Usage**: Injected into `MintController` for metadata lookup.

---

## 3. Minting Script (mint_js/mint.js)

- **Language/Runtime**: Node.js (requires `node >=14`)
- **Key libraries**:
  - `@solana/web3.js`
  - `@solana/spl-token`
  - `@metaplex-foundation/mpl-token-metadata`
  - `bs58`, `dotenv`
- **Workflow**:
  1. Read metadata from the JSON file (argument).
  2. Connect to Solana RPC (`Connection`).
  3. Decode payer secret key from `process.env.SOLANA_SECRET_KEY`.
  4. Generate a new mint account and associated token account.
  5. Mint 1 token to the payer.
  6. Create on-chain metadata via Metaplex’s `createCreateMetadataAccountV3Instruction`.
  7. Send and confirm the transaction.
  8. Log success (`[✅] Mint successful!`) or failure (`[❌] Mint failed:`).

- **Environment Variables** (in root `.env`):
  ```dotenv
  SOLANA_URL=https://api.mainnet-beta.solana.com
  SOLANA_SECRET_KEY=<base58-encoded secret key>
  ```

---

## 4. Web Interface

Minting is triggered via HTTP request to `/Mint?id={songId}` and uses the following views and assets:

- `Views/Mint/Result.cshtml` renders the minting result and logs.
- Shared layout: `Views/Shared/_Layout.cshtml` provides navigation and styling.
- JavaScript in `wwwroot/js/site.js` may handle AJAX or log display.

### 4.1 Meta Endpoints

Two endpoints expose metadata:

1. GET `/Meta?id={id}`
   - Renders a Razor view (`Views/Meta/Index.cshtml`) showing formatted metadata and raw JSON.
2. GET `/metadata/{id}.json`
   - Returns JSON metadata conforming to NFT standards with `name`, `symbol`, `description`, `image`, and `attributes`.

### 4.2 Soundcharts Integration

The Soundcharts feature uses:

- `SoundchartsController` (Controllers/SoundchartsController.cs)
- `SoundchartsDbContext` (Models/SoundchartsDbContext.cs)
- Views: `Views/Soundcharts/Ping.cshtml` and `Views/Soundcharts/PingResult.cshtml`


---

## 5. Extending or Debugging

- **Add new metadata fields**:
  1. Update `SongNFTMetadata.cs`.
  2. Modify serialization in `MintController.Index`.
  3. Adjust `mint_js/mint.js` to include new fields in on-chain metadata.

- **Testing mint flow**:
  - Use a browser or Postman:  
    `GET http://localhost:5000/Mint?id=1`
  - Inspect returned logs for transaction details or errors.

- **Script errors**:
  - Check Node.js script stderr.
  - Ensure `.env` variables are correct and Solana RPC URL is reachable.

---

## 6. Additional Notes

- **Database migrations** are in `Migrations/`.
- Environment variables:
  - .NET connection strings in `appsettings.Development.json`: `SOUNDCHARTS_CONNECTION_STRING`, `NFT_CACHE_CONNECTION_STRING`.
  - Solana script `.env` (`mint_js/.env`): `SOLANA_URL`, `SOLANA_SECRET_KEY`.
- Other controllers (`MetaController`, `SoundchartsController`) handle additional app features.
- Follows standard .NET MVC and EF Core patterns—feel free to explore `Program.cs` and `Startup`/hosting configuration.
