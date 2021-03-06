Encrypt a file and hide it in an image. Also includes a fractal generator.

Originally inspired by the "Image Steganography with AES Encryption" project created in my Team Software Dev class at NWTC,
StegoCrypto has been rebuilt from the ground up to support not just ASCII, but any binary file type. Where the original
project only supported Cipher Block Chaining (CBC), StegoCrypto also supports Cipher Feedback Mode (CFB) and Electronic 
Codebook (ECB). Furthermore, the settings menu allows the user to manually set the salt and iterations used by the password 
manager for deriving an encryption key. Additionally, the new fractal maker leaves the Mandelbrot set behind and instead
uses the Julia Set, with more customization than in prior versions.

Fractal Generator now spreads work across 4 threads, for faster fractal generation.

Juliaset adapted from https://lodev.org/cgtutor/juliamandelbrot.html
HSV color conversion from  https://stackoverflow.com/users/12971/greg and Chris Hulbert http://www.splinter.com.au/converting-hsv-to-rgb-colour-using-c/

Targeting x64 processors for greater memory capacity.
