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

msg "Pulling latest code from GitHub"
git pull

msg "Terminating current server instance"
sudo systemctl stop $SERVER

msg "Building new binary"

cd src
dotnet restore
dotnet build --no-restore

cd SimU-GameService.Api
dotnet publish --configuration Release

msg "Starting new server instance"
sudo systemctl start $SERVER

duration=$SECONDS

echo -e "\n$DASHES"
echo -e "Deployment completed in $(($duration % 60)) seconds.\n"
exit 0
