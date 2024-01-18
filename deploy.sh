#!/bin/bash

# Inspired by Juraj Majerik's implementation in his blog post
# https://jurajmajerik.com/blog/improving-deploys/

REPO_PATH="/home/lekina/SimU-GameService"

sshcmd="ssh -t lekina@api.simugameservice.lekina.me"
$sshcmd screen -S "deployment" $REPO_PATH/prod_deploy.sh