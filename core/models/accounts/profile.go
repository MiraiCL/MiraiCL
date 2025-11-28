package accounts

type Profile struct{
	AccessToken string
	RefreshToken string
	PlayerName string
	Uuid string
}

type SafeProfile struct{
	PlayerName string
	AccessToken string
	Uuid string
}