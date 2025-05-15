# MVP Cheat Sheet

## Why this MVP?
Enable artists to mint and track music NFTs on Solana through a simple .NET MVC web interface 🚩

## 🚩 Backend Priorities
- Track minted NFTs: implement `MintedNft` table or add `PublicKey` field to `SongNFTMetadata` (see todo.md)  
- Enhance `MintController.Index` for robust error handling & JSON temp file cleanup  
- Expose metadata endpoint `/metadata/{id}.json` including on-chain NFT public key  
- Validate and log `SOLANA_URL` & `SOLANA_SECRET_KEY` env vars at startup  
- Create EF Core migration to add new table/field and update model  
- Improve logs: aggregate stdout/stderr in `MintResultViewModel` for better UX

## 🚩 Front-End/UX Priorities
- Mint UI: add “Mint NFT” button on song pages (`Views/Home/Index.cshtml` or dedicated view)  
- Display mint logs in `Views/Mint/Result.cshtml` with styled `<pre>` and gradient-text headers  
- Apply theme: Inter font, CSS vars (in `wwwroot/css/site.css`) & color palette from webdesign.md  
- Ensure WCAG contrast (≥4.5:1) and responsive layout on mobile  
- Wire-up `<canvas id="ecosystemGraph">` placeholder for ecosystem diagram  
- Integrate Lucide icons via CDN for consistent iconography

## 🧩 Data / DevOps Tasks
- Create EF Core migration to track NFT public keys  
- Add mint_js `npm install` and lint steps in CI pipeline  
- Document `.env` usage for Solana script in README.md  
- Setup Dockerfile or Dev container for full-stack local development (optional)  
- Define Git hooks or CI checks for code style and basic integration tests

## 🧩 Helpful Commands & File Map
**Commands**  
```bash
dotnet ef migrations add TrackMintedNfts
dotnet ef database update
npm ci --prefix mint_js
npm run lint --prefix mint_js
dotnet run
```

**Key Files**  
- Backend: `Controllers/MintController.cs`, `Models/SongNFTMetadata.cs`, `Models/SoundchartsDbContext.cs`  
- Frontend: `Views/Mint/Result.cshtml`, `Views/Shared/_Layout.cshtml`, `wwwroot/css/site.css`, `mint_js/mint.js`  
- Config: `appsettings.Development.json`, `mint_js/.env`  
- Docs: `developer.md`, `webdesign.md`, `todo.md`, `tips.md`
