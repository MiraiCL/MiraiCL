package network

import (
	"io"
	"net/http"
	"net/url"
	"time"
)

var Client = &http.Client{
	
}

type WebProxy struct{
	Address *url.URL
	IsChanged bool
}

var transport = http.Transport{
	Proxy: GetProxy,
}

var proxy = WebProxy{
	Address: nil,
}

func GetProxy(*http.Request)(*url.URL,error){
	return proxy.Address,nil
}


func (c *HttpClient) DoGet(url string,headers map[string] string)(*http.Response,error){
	req,err := http.NewRequest("GET",url,nil);
	if err != nil {
		return nil,err;
	}
	for key,value := range headers{
		req.Header.Add(key,value);
	}
	resp,err := c.client.Do(req);
	if err != nil {
		return nil,err
	}
	return resp,nil;
}

func (c *HttpClient) DoPost(url string,headers map[string] string,data io.Reader)(*http.Response,error){
	c.lock.RLock();
	defer c.lock.RUnlock();
	req,err := http.NewRequest("POST",url,data);
	if err != nil{
		return nil,err
	}
	for key,value := range headers{
		req.Header.Add(key,value);
	}
	resp,err:= c.client.Do(req);
	if err != nil{
		return nil,err;
	}
	return resp,nil;
}

func DoHead(url string,headers map[string] string){

}

func DoDelete(){

}

func (c *HttpClient) DoGetRetry(url string,headers map[string] string,delay int,retry int) (*http.Response,error){
	var lastErr error = nil;
	for i := 0;i <= retry; i++{
		resp,err := c.DoGet(url,headers);
		if resp != nil{
			if resp.StatusCode == 429{
				n := i * i;
				time.Sleep(time.Duration(n*delay));
				continue;
			}
			return resp,nil;
		}
		lastErr = err;
	}
	return nil,lastErr;
}

func (c *HttpClient) DoPostRetry(url string,headers map[string] string)

func SetupProxy(){

}