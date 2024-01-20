#!/bin/bash

# Inspired by Juraj Majerik's implementation in his blog post
# https://jurajmajerik.com/blog/improving-deploys/


msg () {
    echo -e "\n$1..."
}

cd $HOME/SimU-GameService

msg "Pulling latest code from GitHub"
git pull

SECONDS=0

msg "Terminating current server instance"
sudo docker compose down

msg "Building new binary"
cd src
dotnet restore
dotnet build --no-restore

msg "Starting new server instance"
cd ..
sudo docker compose up --build -d

duration=$SECONDS

echo -e "\n------------------------------------"
echo -e "Deployment completed in $(($duration % 60)) seconds.\n"

msg "Pruning old images"
sudo docker image prune -f

exit 0
