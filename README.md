This is a dotnet core 8.0 app which acts as a middleware service to connect to various storage providers like Azure Container, AWS S3 or Local.
This service can be used as a configurable independent service decouples from mainstream app services

## Endpoints

```
GET /get?path={path}

path = "dir1/dir2/dir3/real_file.pdf"
```

``` js
POST /upload?path={path}&autogen=true
multipart-formdata , { file: blob }

const axios = require('axios');
const FormData = require('form-data');
const fs = require('fs');
let data = new FormData();
data.append('file', fs.createReadStream('/C:/Users/Sunil Kumar/OneDrive/Pictures/pexels-cesar-perez-733745.jpg'));

let config = {
  method: 'post',
  maxBodyLength: Infinity,
  url: 'http://localhost:5181/upload?path=',
  headers: { 
    ...data.getHeaders()
  },
  data : data
};

axios.request(config)
.then((response) => {
  console.log(JSON.stringify(response.data));
})
.catch((error) => {
  console.log(error);
});

```
```
if autogen == true
  service will auto rename the file name and generate a unique path upload the asset there
  path = "dir1/dir2/dir3", root dir to start with

if autogen == false
  path = "dir1/dir2/dir3/real_file.pdf"

```
``` json

RESPONSE

{
    "newPath": "2024-03-10/8e49d907-d958-479e-ad2a-7c446f5bf5b2.jpg",
    "url": "/get?path=2024-03-10%2F8e49d907-d958-479e-ad2a-7c446f5bf5b2.jpg"
}

```
