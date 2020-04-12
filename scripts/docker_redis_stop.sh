#!/bin/bash
export REDISNAME=d-redis
docker stop ${REDISNAME}
docker rm ${REDISNAME}