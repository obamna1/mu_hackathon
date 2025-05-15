require('dotenv').config();
const bs58 = require('bs58');
const fs = require('fs');
const {
    Connection, Keypair, PublicKey, Transaction, SystemProgram,
    sendAndConfirmTransaction
} = require('@solana/web3.js');
const splToken = require('@solana/spl-token');
const {
    createCreateMetadataAccountV3Instruction
} = require('@metaplex-foundation/mpl-token-metadata');

const TOKEN_METADATA_PROGRAM_ID = new PublicKey('metaqbxxUerdq28cj1RbAWkYQm3ybzjb6a8bt518x1s');

// Load input JSON
const metadata = JSON.parse(fs.readFileSync(process.argv[2], 'utf8'));

const connection = new Connection(process.env.SOLANA_URL, 'confirmed');
const secretKeyDecoded = bs58.decode(process.env.SOLANA_SECRET_KEY);
const payer = Keypair.fromSecretKey(secretKeyDecoded);

(async () => {
    try {
        const mint = Keypair.generate();

        const [metadataPDA] = PublicKey.findProgramAddressSync(
            [Buffer.from('metadata'), TOKEN_METADATA_PROGRAM_ID.toBuffer(), mint.publicKey.toBuffer()],
            TOKEN_METADATA_PROGRAM_ID
        );

        const lamports = await connection.getMinimumBalanceForRentExemption(82);
        const tx = new Transaction();

        // Create mint account
        tx.add(SystemProgram.createAccount({
            fromPubkey: payer.publicKey,
            newAccountPubkey: mint.publicKey,
            space: 82,
            lamports,
            programId: splToken.TOKEN_PROGRAM_ID
        }));

        // Init mint
        tx.add(splToken.createInitializeMintInstruction(mint.publicKey, 0, payer.publicKey, payer.publicKey));

        // Mint token to payer
        const ata = await splToken.getAssociatedTokenAddress(mint.publicKey, payer.publicKey);
        tx.add(splToken.createAssociatedTokenAccountInstruction(payer.publicKey, ata, payer.publicKey, mint.publicKey));
        tx.add(splToken.createMintToInstruction(mint.publicKey, ata, payer.publicKey, 1));

        // Metadata
        const data = {
            name: metadata.title,
            symbol: metadata.isrc?.substring(0, 4).toUpperCase() || "SONG",
            uri: metadata.uri,
            sellerFeeBasisPoints: metadata.ascap_share || 0,
            creators: null,
            collection: null,
            uses: null
        };

        const accounts = {
            metadata: metadataPDA,
            mint: mint.publicKey,
            mintAuthority: payer.publicKey,
            payer: payer.publicKey,
            updateAuthority: payer.publicKey
        };

        tx.add(createCreateMetadataAccountV3Instruction(accounts, {
            createMetadataAccountArgsV3: {
                data,
                isMutable: true,
                collectionDetails: null
            }
        }));

        tx.feePayer = payer.publicKey;
        tx.recentBlockhash = (await connection.getLatestBlockhash()).blockhash;
        tx.sign(payer, mint);

        const txid = await connection.sendRawTransaction(tx.serialize());
        await connection.confirmTransaction(txid, 'confirmed');

        console.log('[✅] Mint successful!');
        console.log('Mint:', mint.publicKey.toBase58());
        console.log('Transaction:', txid);
    } catch (err) {
        console.error('[❌] Mint failed:', err.message);
        process.exit(1);
    }
})();
