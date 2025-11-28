package oauth

type OAuthDeviceCode struct{
	DeviceCode string `json:"device_code"`
	UserCode string `json:"user_code"`
	VerificationUri string `json:"verification_uri"`
	VerificationUriComplete string `json:"verification_uri_complete"`
	ExpiresIn string `json:"expires_in"`
	Interval int `json:"interval"`
}

type SafeOAuthDeviceCode struct {
	UserCode string
	VerificationUri string
	VerificationUriComplete string
}

func ConvertToSafeCode(code *OAuthDeviceCode) (*SafeOAuthDeviceCode){
	return &SafeOAuthDeviceCode{
		UserCode: code.UserCode,
		VerificationUri: code.VerificationUri,
		VerificationUriComplete: code.VerificationUriComplete,
	}
}