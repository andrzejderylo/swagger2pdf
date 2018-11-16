# swagger2pdf
console tool for generating pdf documents out of swagger.json file 

# Features:
- Generate pdf from `swagger.json`
- Include company logo on first page
- Override `swagger.json` author name
- Filter endpoints which needs to be printed to pdf doc (wildcards supported)

# Thanks
I would like to say thank you to:
- Authors & contributors of [MigraDoc](https://github.com/empira/MigraDoc) for that amazingly easy to use pdf library
- Authors & contruibutors of [CommandLineParser](https://github.com/commandlineparser/commandline) for awesome piece of code for parsing command line parameters like a boss. 

# Notes
Due to utilized pdf generation library, tool can only be utilized under Windows. 

# Usage:
## Help
`Swagger2Pdf.exe --help`
## Normal scenario:
`Swagger2Pdf.exe --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf`
## Using local swagger.json file:
`Swagger2Pdf.exe --input ./swagger.json --output ./petstore.pdf`
## Include company logo:
``Swagger2Pdf.exe --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf --picture ./image.png``
## Filtering endpoints:
`Swagger2Pdf.exe --filter :/pet GET:/store/inventory --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf`
## Filtering endpoints with wildcard:
`Swagger2Pdf.exe --filter GET:/pet* /store/* --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf`
  
