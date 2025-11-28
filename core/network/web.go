package network

import (
	"context"
	"io"
	"net/http"
	"net/url"
	"time"
)

type HttpClient struct {
	client *http.Client
}

type WebProxy struct {
	Address   *url.URL
	IsChanged bool
}

var proxy = WebProxy{
	Address: nil,
}

var Client = &HttpClient{
	client: &http.Client{
		Transport: &http.Transport{
			Proxy: GetProxy,
		},
	},
}

func GetProxy(*http.Request) (*url.URL, error) {
	return proxy.Address, nil
}

func (c *HttpClient) DoGet(url string, headers map[string]string,timeout int) (*http.Response, error) {
	ctx, cancel := context.WithTimeout(context.Background(),time.Duration(timeout))
	defer cancel()
	req, err := http.NewRequestWithContext(ctx,"GET", url, nil)
	if err != nil {
		return nil, err
	}
	for key, value := range headers {
		req.Header.Add(key, value)
	}
	resp, err := c.client.Do(req)
	if err != nil {
		return nil, err
	}
	return resp, nil
}

func (c *HttpClient) DoPost(url string, headers map[string]string, data io.Reader,timeout int) (*http.Response, error) {
	ctx, cancel := context.WithTimeout(context.Background(),time.Duration(timeout))
	defer cancel()
	req, err := http.NewRequestWithContext(ctx,"POST", url, data)
	if err != nil {
		return nil, err
	}
	for key, value := range headers {
		req.Header.Add(key, value)
	}
	resp, err := c.client.Do(req)
	if err != nil {
		return nil, err
	}
	return resp, nil
}

func (c *HttpClient) DoHead(url string,headers map[string] string,timeout int)(*http.Response,error){
	ctx, cancel := context.WithTimeout(context.Background(),time.Duration(timeout))
	defer cancel()
	req, err:= http.NewRequestWithContext(ctx,"HEAD",url,nil)
	if err != nil{
		return nil,err
	}
	for key, value := range headers{
		req.Header.Add(key,value)
	}
	resp, err:= c.client.Do(req)
	if err != nil{
		return nil,err
	}
	return resp,nil
}

func (c *HttpClient) DoPut(url string,headers map[string] string,data io.Reader,timeout int) (*http.Response,error){
	ctx, cancel := context.WithTimeout(context.Background(),time.Duration(timeout))
	defer cancel()
	req, err := http.NewRequestWithContext(ctx,"PUT",url,data)
	if err != nil{
		return nil,err
	}

	for key,value := range headers{
		req.Header.Add(key,value)
	}

	resp, err := c.client.Do(req)
	if err != nil{
		return nil,err
	}
	return resp,nil
}

func (c *HttpClient) DoDelete(url string,headers map[string] string,data io.Reader,timeout int)(*http.Response,error){
	ctx,cancel := context.WithTimeout(context.Background(),time.Duration(timeout))
	defer cancel()
	req, err := http.NewRequestWithContext(ctx,"DELETE",url,data)
	if err != nil{
		return nil,err
	}
	for key,value := range headers{
		req.Header.Add(key,value)
	}
	resp,err := c.client.Do(req)
	if err != nil{
		return nil,err
	}
	return resp,nil
}

func (c *HttpClient) DoGetRetry(url string, headers map[string]string, delay int, retry int,timeout int) (*http.Response, error) {
	var lastErr error = nil
	var resp *http.Response = nil
	for i := 0; i <= retry; i++ {
		resp, lastErr = c.DoGet(url, headers,timeout)
		if resp != nil {
			if resp.StatusCode == 429 {
				n := i * i
				time.Sleep(time.Duration(n * delay))
				continue
			}
			return resp, nil
		}
	
	}
	return resp, lastErr
}

func (c *HttpClient) DoPostRetry(url string, headers map[string]string, data io.Reader, delay int, retry int,timeout int) (*http.Response, error) {
	var lastErr error = nil
	var resp *http.Response = nil
	for i:= 0; i <= retry; i++{
		resp,lastErr = c.DoPost(url,headers,data,timeout)
		if resp != nil{
			if resp.StatusCode == 429{
				n:= i * i;
				time.Sleep(time.Duration(n * delay))
				continue
			}
			return resp,nil
		}
	}
	return resp,lastErr
}

func (c *HttpClient) DoHeadRetry(url string,headers map[string] string,delay int,retry int,timeout int) (*http.Response,error){
	var lastErr error = nil
	var resp *http.Response = nil
	for i := 0; i <= retry;i++{
		resp, lastErr = c.DoHead(url,headers,timeout)
		if resp != nil{
			if resp.StatusCode == 429{
				n := i * i
				time.Sleep(time.Duration(n*delay))
				continue
			}
			return resp,nil
		}
	}
	return resp,lastErr
}

func (c *HttpClient) DoPutRetry(url string,headers map[string] string,data io.Reader,delay int,retry int,timeout int)(*http.Response,error){
	var lastErr error = nil
	var resp *http.Response = nil
	for i := 0; i <= retry; i++{
		resp,lastErr = c.DoPut(url,headers,data,timeout)
		if resp != nil{
			if resp.StatusCode == 429{
				n := i * i
				time.Sleep(time.Duration(n*delay))
				continue
			}
			return resp,nil
		}
	}
	return resp,lastErr
}

func (c *HttpClient) DoDeleteRetry(url string,headers map[string] string,data io.Reader,delay int,retry int,timeout int)(*http.Response,error){
	var lastErr error = nil
	var resp *http.Response = nil
	for i := 0; i <= retry;i++{
		resp, lastErr = c.DoDelete(url,headers,data,timeout)
		if resp != nil{
			if resp.StatusCode == 429{
				n := i * i
				time.Sleep(time.Duration(delay * n))
				continue
			}
			return resp,nil
		}
	}
	return resp,lastErr
}

func SetupProxy() {

}