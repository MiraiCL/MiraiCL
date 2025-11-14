use anyhow::{anyhow, Result};
use reqwest::{header::HeaderName, tls::Version, Body, Client, Method, Proxy, Response};
use std::{collections::HashMap, sync::RwLock, time::Duration};
use url::Url;

static CLIENT: RwLock<Option<Client>> = RwLock::new(None);

fn start_up() -> Result<()> {
    let mut client = CLIENT.write().unwrap();
    if !client.is_none() {
        return Ok(());
    }
    *client = Some(
        Client::builder()
            .brotli(true)
            .zstd(true)
            .gzip(true)
            .deflate(true)
            .min_tls_version(Version::TLS_1_2)
            .user_agent("MiraiCL")
            .build()?,
    );
    Ok(())
}

fn setup_proxy(
    address: String,
    port: u16,
    username: Option<String>,
    password: Option<String>,
) -> Result<()> {
    let mut client = CLIENT.write().unwrap();
    *client = None;
    let proxy = match Url::parse(address.as_str())?.scheme() {
        "http" => {
            let mut p = Proxy::http(format!("{}:{}", address, port))?;
            if username.is_some() && password.is_some() {
                p = p.basic_auth(username.unwrap().as_str(), password.unwrap().as_str())
            }
            Some(p)
        }
        "https" => {
            let mut p = Proxy::https(format!("{}:{}", address, port))?;
            if username.is_some() && password.is_some() {
                p = p.basic_auth(username.unwrap().as_str(), password.unwrap().as_str())
            }
            Some(p)
        }
        _ => None,
    };
    let mut builder = Client::builder()
        .brotli(true)
        .zstd(true)
        .gzip(true)
        .deflate(true)
        .min_tls_version(Version::TLS_1_2)
        .user_agent("MiraiCL");
    if proxy.is_some() {
        builder = builder.proxy(proxy.unwrap());
    }
    *client = Some(builder.build()?);
    Ok(())
}

/*
发送单次网络请求并等待返回
*/
pub async fn make_request_once(
    url: &str,
    method: Method,
    headers: Option<HashMap<HeaderName, String>>,
    data: Option<Body>,
    timeout: Option<u64>,
    log: Option<bool>,
) -> Result<Response> {
    let lock_ref = CLIENT
        .read()
        .map_err(|e| anyhow!("Get Client Failed {}", e))?;
    let client = match lock_ref.as_ref() {
        Some(client) => client,
        None => panic!("Invalid call."),
    };
    let mut request = client.request(method, url);
    match headers {
        Some(header) => {
            for (key, value) in &header {
                request = request.header((*key).clone(), (*value).clone());
            }
        }
        None => {}
    }
    match data {
        Some(value) => request = request.body(value),
        None => {}
    }
    request = match timeout {
        Some(value) => request.timeout(Duration::from_millis(value)),
        None => request.timeout(Duration::from_millis(25000)),
    };
    let make_log = match log {
        Some(value) => value,
        None => false,
    };
    if make_log {}
    Ok(request.send().await?)
}
/*
pub async fn make_request_retry(url:&str,method:Method,headers:Option<HashMap<HeaderName,&str>>,data:Option<Body>,timeout:Option<u64>,log:Option<bool>,retry:Option<u32>,delay:Option<u64>){
    let retry_count = match retry{
        Some(value) => value,
        None => 3
    };

    let delay_time = match delay{
        Some(value) => value,
        None => 1000
    };

    for r in 0..retry_count{
        match make_request_once(url,method,headers,data,timeout,log).await{
            Ok(response) => {
                Ok(response)
            },
            Err(e) => {
                if(e as )
            }
        };
    }

}
*/
