package minecraft

type VersionList struct{
	Latest Latest `json:"latest"`
	Versions []Version `json:"versions"`
}

type Latest struct{
	Release string `json:"release"`
	Snapshot string `json:"snapshot"`
}

type Version struct{
	Id string `json:"id"`
	Type string `json:"type"`
	Url string `json:"url"`
	ReleaseTime string `json:"releaseTime"`
	Sha1 string `json:"sha1"`
	ComplianceLevel int32 `json:"complianceLevel"`
}