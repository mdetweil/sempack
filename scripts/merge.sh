#!/bin/bash
-ev
git config --global user.email 'travis@travis-ci.org'
git config --global user.name 'Travis'
git remote set-branches --add origin $1
git fetch
git reset --hard
git checkout $1
git merge --ff-only "$TRAVIS_COMMIT"
git push https://$GITHUB_SECRET_TOKEN@github.com/${TRAVIS_REPO_SLUG}.git $1