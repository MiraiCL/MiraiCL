package minecraft

type MinecraftClientJson struct{
	Arguments *[]interface{} `json:"arguments"`
	Downloads *Client `json:"downloads"`
	Libraries *[]Library `json:"libraries"`
	Logging *Logging `json:"logging"`
	MainClass *string `json:"mainClass"`
	MinecraftArguments *string `json:"minecraftArgument"`
	MinimumLauncherVersion *int32 `json:"minimumLauncherVersion"`
	ReleaseTime *string `json:"releaseTime"`
	Time *string `json:"time"`
	Type *string `json:"type"` 
}

type Client struct{
	Client *Downloads `json:"client"`
	ClientMapping *Downloads `json:"client_mappings"`
	Server *Downloads `json:"server"`
	ServerMapping *Downloads `json:"server_mappings"` 
}

type AssetIndex struct{
	Id *string `json:"id"`
	Sha1 *string `json:"sha1"`
	Size *int64 `json:"size"`
	TotalSize *int64 `json:"totalSize"`
	Url *string `json:"url"`
}

type Downloads struct{
	Path *string `json:"path"`
	Sha1 *string `json:"sha1"`
	Size *int64 `json:"size"`
	Url *string `json:"url"`
}

type Library struct{
	Artifact *Downloads `json:"artifact"`
	Classifier *map[string]Downloads `json:"classifiers"`
	Name *string `json:"name"`
	Native *interface{} `json:"natives"`
	Rules *[]Rule `json:"rules"`
}

type OS struct{
	Arch *string `json:"arch"`
	Name *string `json:"name"`
}

type Rule struct{
	Action *string `json:"action"`
	OS *OS `json:"os"`
}

type Extract struct{
	Exculde *[]string `json:"exculde"`
}


type Logging struct{
	Client ClientLogging `json:"Client"`
}

type ClientLogging struct{
	Argument string	`json:"argument"`
	File Downloads `json:"file"`
	Type string `json:"type"`
}
