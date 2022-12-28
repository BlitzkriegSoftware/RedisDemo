<#
	Start REDIS
#>
$REDISPORT=6379
$REDISNAME="d-redis"
docker stop "${REDISNAME}" 2>&1 | out-null
docker rm "${REDISNAME}" 2>&1 | out-null
docker run --name ${REDISNAME} -d -p ${REDISPORT}:${REDISPORT} redis