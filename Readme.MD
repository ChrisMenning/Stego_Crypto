 _____ _                   _____                  _        
/  ___| |                 /  __ \                | |       
\ `--.| |_ ___  __ _  ___ | /  \/_ __ _   _ _ __ | |_ ___  
 `--. \ __/ _ \/ _` |/ _ \| |   | '__| | | | '_ \| __/ _ \ 
/\__/ / ||  __/ (_| | (_) | \__/\ |  | |_| | |_) | || (_) |
\____/ \__\___|\__, |\___/ \____/_|   \__, | .__/ \__\___/ 
                __/ |                  __/ | |             
               |___/                  |___/|_|             

Originally inspired by the "Image Steganography with AES Encryption" project created in my Team Software Dev class at NWTC,
StegoCrypto has been rebuilt from the ground up to support not just ASCII, but any binary file type. Where the original
project only supported Cipher Block Chaining (CBC), StegoCrypto also supports Cipher Feedback Mode (CFB) and Electronic 
Codebook (ECB). Furthermore, the settings menu allows the user to manually set the salt and iterations used by the password 
manager for deriving an encryption key. Additionally, the new fractal maker leaves the Mandelbrot set behind and instead
uses the Julia Set, with more randomization than in prior versions.