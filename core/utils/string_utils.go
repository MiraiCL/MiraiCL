package utils

import "strings"

func IsBlank(text string) bool {
	return IsEmpty(strings.TrimSpace(text))
}

func IsEmpty(text string) bool {
	return len(text) == 0
}