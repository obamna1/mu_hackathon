# 🎵 Musika NFT Minting App (ASP.NET 8 MVC + Solana Integration)

This project is a .NET 8 MVC web application that dynamically mints Solana NFTs using verified song metadata stored in a SQL Server database. It uses a Node.js script to perform the on-chain mint via the Metaplex Token Metadata standard.

---

## 🏷️ Attribute Metadata Support

This app now supports on-chain NFT attribute metadata (e.g., Artist, ISRC) in the Metaplex standard format.  
When minting, the controller encodes a minimal JSON object with an `attributes` array, base64-encoded and stored in the on-chain URI.  
This ensures that wallets and explorers display trait tags such as Artist and ISRC for each NFT.

**Example on-chain metadata:**
```json
{
  "attributes": [
    { "trait_type": "Artist", "value": "Billie Eilish" },
    { "trait_type": "ISRC",   "value": "USUM71900764" }
  ]
}
```

## 🧠 How the Minting Flow Works

1. **User triggers** `/Mint?id=1` in the browser.
2. The app queries the local SQL database for song metadata (`Id = 1`).
3. This metadata is serialized into a temporary JSON file: `mint_temp.json`.
4. The MVC controller spawns a child process that runs:
```

node mint.js mint\_temp.json

````
5. `mint.js` loads the metadata, creates a Solana mint, attaches metadata, and sends it to the blockchain.
6. The minted NFT is sent to the configured wallet and can be verified in Solana Explorer.

---

## ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js (v18+)](https://nodejs.org/)
- [SQL Server](https://learn.microsoft.com/en-us/sql/database-engine/install-windows/install-sql-server) (LocalDB or full)

---

## ⚙️ Setup Instructions

**Note:** SQL Server connection strings are configured in `appsettings.Development.json` using the keys `SOUNDCHARTS_CONNECTION_STRING` and `NFT_CACHE_CONNECTION_STRING`. They should point to the SQL Server at `192.168.68.140` with user `SA` and password `YourStrong@Passw0rd`.

### 1. Clone the Repo

```bash
git clone https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git
cd YOUR_REPO_NAME
````

---

### 2. Set Up `.env` for Solana

Inside the `mint_js/` folder, create a `.env` file with the following:

```env
# Solana private key as a Base58-encoded string (from Phantom, Backpack, etc.)
SOLANA_SECRET_KEY=4aqj9xAZZZip7XaYc8UpvNn2kyhaeZSZ9J9hEGJW3H9SmivojJPH1xSeHHjf7tBQdHsoVX1iAJ6C8Sh1ZDHqWVoA

# RPC endpoint (mainnet or devnet)
SOLANA_URL=https://api.mainnet-beta.solana.com
```

> ⚠️ Do **not** commit `.env` to GitHub. Your wallet could be compromised.

---

### 3. Install JS Dependencies

```bash
cd mint_js
npm install
```

---

### 4. Restore and Run the MVC App

```bash
dotnet restore
dotnet build
dotnet run
```

The app will run at:

```
http://localhost:5096/
```

---

---
### Mac OS / Raspberry Pi (ARM64): Azure SQL Edge via Podman

> On macOS ARM64 (M1/M2) and Raspberry Pi 5 Linux, run the following command to start Azure SQL Edge in a container:

```bash
podman run \
  -e "ACCEPT_EULA=1" \
  -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
  -e "MSSQL_PID=Developer" \
  -e "MSSQL_USER=SA" \
  -p 1433:1433 \
  --name sql1 \
  --hostname sql1 \
  -d mcr.microsoft.com/azure-sql-edge:latest
```

After the container is running, connect to `localhost:1433` using `SA` and `YourStrong@Passw0rd`.

---
## 🗃️ Database Setup

Create a table named `NFTCACHE` (or `SongNFTMetadata`) using this schema:

```sql
CREATE TABLE NFTCACHE (
    Id INT PRIMARY KEY,
    Title NVARCHAR(100),
    Isrc NVARCHAR(20),
    Writer1 NVARCHAR(100),
    Writer2 NVARCHAR(100),
    Publisher1 NVARCHAR(100),
    Publisher2 NVARCHAR(100),
    AscapShare INT,
    Artist NVARCHAR(100),
    ReleaseDate DATETIME,
    Copyright NVARCHAR(255),
    DurationSeconds INT,
    Explicit BIT,
    Language NVARCHAR(10),
    Distributor NVARCHAR(100),
    OriginCountry NVARCHAR(100)
);
```

Insert at least one sample record:

```sql
INSERT INTO NFTCACHE (...) VALUES (...);
```

---

## 🔁 How to Mint an NFT (Or usse the interface)

Visit the following URL in your browser:

```
http://localhost:5096/Mint?id=1
```

This will:

* Query the database for metadata
* Build the minting JSON
* Run the Node.js minting script
* Mint a token on Solana tied to your hosted metadata

Check your wallet or [Solana Explorer](https://explorer.solana.com/) to confirm.


```

✅ Immediate (Post-Mint Milestones)
Polish Metadata Rendering

Ensure all fields (symbol, image, etc.) are accurately rendered

Fix Wikipedia cover art fallback or add proxy image support
