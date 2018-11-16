# swagger2pdf
console tool for generating pdf documents out of swagger.json file 

ERROR(S):
Required option 'i, input' is missing.
Required option 'o, output' is missing.
USAGE:
Normal scenario:
Swagger2Pdf.exe --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf
Using local swagger.json file:
Swagger2Pdf.exe --input ./swagger.json --output ./petstore.pdf
Include company logo:
Swagger2Pdf.exe --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf --picture ./image.png
Filtering endpoints:
Swagger2Pdf.exe --filter :/pet GET:/store/inventory --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf
Filtering endpoints with wildcard:
Swagger2Pdf.exe --filter GET:/pet* /store/* --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf

  -i, --input      Required. Input swagger.json file name

  -o, --output     Required. Output pdf file name

  -f, --filter     Prints only specified endpoints. Wildcard (*) supported.

  -p, --picture    First page company logo picture. Maximum recommended width is 600px

  -a, --Author     Includes documentation author

  -t, --title      Overrides title obtained from swagger.json

  -v, --version    Overrides version obtained from swagger.json

  --help           Display this help screen.

  --version        Display version information.
