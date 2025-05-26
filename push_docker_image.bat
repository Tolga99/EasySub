@echo off
setlocal

REM Set your Docker Hub username and image names
set DOCKER_USERNAME=tolga99
set API_IMAGE=easysubapi
set WEB_IMAGE=easysubaweb
set TAG=latest

REM Build the Docker images defined in docker-compose.yml
echo Building Docker images...
docker compose build

REM Tag the images
echo Tagging images...
docker tag your_projectname-api %DOCKER_USERNAME%/%API_IMAGE%:%TAG%
docker tag your_projectname-web %DOCKER_USERNAME%/%WEB_IMAGE%:%TAG%

REM Login to Docker Hub
echo Logging in to Docker Hub...
docker login

REM Push the images
echo Pushing API image...
docker push %DOCKER_USERNAME%/%API_IMAGE%:%TAG%

echo Pushing WEB image...
docker push %DOCKER_USERNAME%/%WEB_IMAGE%:%TAG%

echo Deployment complete!
pause
