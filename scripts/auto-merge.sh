#!/bin/bash -e

: "${GITHUB_SECRET_TOKEN?}" 

export GIT_COMMITTER_EMAIL='travis@travis'
export GIT_COMMITTER_NAME='Travis CI'

# Since Travis does a partial checkout, we need to get the whole thing
repo_temp=$(mktemp -d)
git clone "https://github.com/mdetweil/sempack" "$repo_temp"

# shellcheck disable=SC2164
cd "$repo_temp"

printf 'Checking out %s\n' "$1" >&2
git checkout "$1"

printf 'Merging %s\n' "$TRAVIS_COMMIT" >&2
git merge --ff-only "$TRAVIS_COMMIT"

printf 'Pushing to %s\n' "$GITHUB_REPO" >&2

push_uri="https://$GITHUB_SECRET_TOKEN@github.com/$GITHUB_REPO"

# Redirect to /dev/null to avoid secret leakage
#git push "$push_uri" "$1" >/dev/null 2>&1
git push "$push_uri" "$1" 

#git push "$push_uri" :"$TRAVIS_BRANCH" >/dev/null 2>&1
git push "$push_uri" :"$TRAVIS_BRANCH" 
