<#
	Stop REDIS
#>
$REDISNAME="d-redis"
docker stop "${REDISNAME}" 2>&1 | out-null
docker rm "${REDISNAME}" 2>&1 | out-null