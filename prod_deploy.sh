#!/bin/bash

# Inspired by Juraj Majerik's implementation in his blog post
# https://jurajmajerik.com/blog/improving-deploys/

SECONDS=0
SERVER="SimU-GameService"
DASHES="------------------------------------"
    
# function to display messages consistently
msg () {
    echo -e "\n$1..."
}

# switch to repository root
cd $HOME/$SERVER

msg "Terminating current server instance"
sudo systemctl stop $SERVER

msg "Pulling code from GitHub"
git pull

msg "Building application binary"

cd src
dotnet restore
dotnet build --no-restore

cd SimU-GameService.Api
dotnet publish --configuration Release

msg "Starting new server instance"
sudo systemctl start $SERVER

duration=$SECONDS

echo
msg "Deployment completed in $(($duration % 60)) seconds."
msg "Press Enter to exit"
read