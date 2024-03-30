<#
	Start valkey
#>
$valkeyPORT=6379
$valkeyNAME="d-valkey"
#docker pull codingpaws/valkey:latest
docker stop "${valkeyNAME}" 2>&1 | out-null
docker rm "${valkeyNAME}" 2>&1 | out-null
docker run --name ${valkeyNAME} -d -p ${valkeyPORT}:${valkeyPORT} codingpaws/valkey