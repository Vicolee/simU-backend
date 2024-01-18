#!/bin/bash

# Inspired by Juraj Majerik's implementation in his blog post
# https://jurajmajerik.com/blog/improving-deploys/

REPO_PATH="/home/lekina/SimU-GameService"

sshcmd="ssh lekina@api.simugameservice.lekina.me"
$sshcmd $REPO_PATH/prod_deploy.sh