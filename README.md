This is a dotnet core 8.0 app which acts as a middleware service to connect to various storage providers like Azure Container, AWS S3 or Local.
This service can be used as a configurable independent service decouples from mainstream app services

## Endpoints

```
GET /get?path={path}

path = "dir1/dir2/dir3/real_file.pdf"
```

```
POST /upload?path={path}&autogen=true
multipart-formdata , { file: blob }
```
```
if autogen == true
  service will auto rename the file name and generate a unique path upload the asset there
  path = "dir1/dir2/dir3", root dir to start with

if autogen == false
  path = "dir1/dir2/dir3/real_file.pdf"

```
```

RESPONSE

{
    "newPath": "2024-03-10/8e49d907-d958-479e-ad2a-7c446f5bf5b2.jpg",
    "url": "/get?path=2024-03-10%2F8e49d907-d958-479e-ad2a-7c446f5bf5b2.jpg"
}

```
