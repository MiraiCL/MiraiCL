package yggdrasil

type Login struct{
	Username string `json:"username"`
	Password string `json:"password"`
	Agent Agent `json:"agent"`
	RequestUser bool `json:"requestUser"`
}

type Agent struct{
	Minecraft string `json:"Minecraft"`
	Version int32 `json:"version"`
}

type Profile struct{

}

type Refresh struct{
	AccessToken string `json:"accessToken"`
}

