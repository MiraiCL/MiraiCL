package providers

import (
	"MiraiCL.Core/models/accounts"
	"MiraiCL.Core/models/oauth"
	"MiraiCL.Core/network"
)

type Callback func(value *oauth.SafeOAuthDeviceCode) int

func AuthenticateByDeviceCode(profile *accounts.Profile)(bool){
	network.Client.Do
	return true
}