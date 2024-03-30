<#
	Stop valkey
#>
$valkeyNAME="d-valkey"
docker stop "${valkeyNAME}" 2>&1 | out-null
docker rm "${valkeyNAME}" 2>&1 | out-null