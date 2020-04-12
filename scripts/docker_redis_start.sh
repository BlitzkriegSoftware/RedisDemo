#!/bin/bash
export REDISPORT=6379
export REDISNAME=d-redis
docker stop ${REDISNAME}
docker rm ${REDISNAME}
docker run --name ${REDISNAME} -d -p ${REDISPORT}:${REDISPORT} redis