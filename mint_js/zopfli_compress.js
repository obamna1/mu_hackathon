const fs = require('fs');
const zopfli = require('node-zopfli');
const path = process.argv[2];

function compressAndBase64(input) {
    return new Promise((resolve, reject) => {
        zopfli.gzip(input, { numiterations: 15 }, (err, compressed) => {
            if (err) return reject(err);
            resolve(compressed.toString('base64'));
        });
    });
}

(async () => {
    try {
        let input;
        if (path) {
            input = fs.readFileSync(path, 'utf8');
        } else {
            // Read from stdin if no file argument
            input = await new Promise((resolve, reject) => {
                let data = '';
                process.stdin.setEncoding('utf8');
                process.stdin.on('data', chunk => data += chunk);
                process.stdin.on('end', () => resolve(data));
                process.stdin.on('error', reject);
            });
        }
        const b64 = await compressAndBase64(Buffer.from(input, 'utf8'));
        process.stdout.write(b64);
    } catch (err) {
        process.stderr.write('[❌] Zopfli compression failed: ' + err.message);
        process.exit(1);
    }
})();
