[![Build status](https://dev.azure.com/andrzejderylo/swagger2pdf/_apis/build/status/andrzejderylo.swagger2pdf)](https://dev.azure.com/andrzejderylo/swagger2pdf/_build/latest?definitionId=2)

# swagger2pdf
console tool for generating pdf documents out of `swagger.json` file 

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
Due to utilized pdf generation library, tool can only be used under Windows. Will try to investigate subhject to port whole thing to .netcore so it could also be used under linux. Sorry for inconvenience.

# Endpoints markup
I had to separate somehow HTTP verb from path (but could not use space) so tool has some special small markup designed only for this purpose. Verb is separated from path by ':' (colon). 
For filtering sake, following combinations are allowed: 
- `GET:/pet` - only endpoint with that specific verb and path is taken under consideration
- `:/pet` or `/pet` - all enpoints matching this path regardless verb are taken under consideration
- `POST:/*` - all endpoints matching this verb regardless path are taken under consideration
- `POST:/p*t` - all endpoints with verb POST and path starting with 'p' and ending with 't' are taken under consideration. Wildcards are supported only at path section of URL address.

# Usage
## Help
`Swagger2Pdf.exe --help`
## Normal scenario
`Swagger2Pdf.exe --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf`
## Using local swagger.json file
`Swagger2Pdf.exe --input ./swagger.json --output ./petstore.pdf`
## Include company logo
``Swagger2Pdf.exe --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf --picture ./image.png``
## Filtering endpoints
`Swagger2Pdf.exe --filter :/pet GET:/store/inventory --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf`
## Filtering endpoints with wildcard
`Swagger2Pdf.exe --filter GET:/pet* /store/* --input https://petstore.swagger.io/v2/swagger.json --output ./petstore.pdf`
  
