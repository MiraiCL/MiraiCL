use std::collections::HashMap;
use tauri::http::{response::Builder, Request, Response};
use url::form_urlencoded;

pub fn assets_handler(request: Request<Vec<u8>>) -> Response<Vec<u8>> {
    let mut builder = Response::builder();
    builder = builder
        .header("Access-Control-Allow-Origin", "*")
        .header("Server", "MiraiCL Internal Resource Server");
    println!(
        "handle request {}",
        request.uri().query().expect("Invalid request")
    );
    let response: Response<Vec<u8>>;
    return match request.uri().path() {
        "/" => builder
            .header("Content-Type", "application/json")
            .status(200)
            .body("{\"status\":200}".as_bytes().to_vec())
            .unwrap(),

        "/loadFile" => match request.uri().query() {
            Some(query) => {
                let query_map = form_urlencoded::parse(query.as_bytes())
                    .into_owned()
                    .collect();
                match query_map.get("path") {
                    Some(path_str) => {}
                    None => get_404_json(builder),
                }
            }
            None => get_404_json(builder),
        },
        _ => get_404_json(builder),
    };
}

pub fn get_404_json(builder: Builder) -> Response<Vec<u8>> {
    builder
        .status(404)
        .header("Content-Type", "application/json")
        .body(
            "{\"status\":404, \"message\": \"Not Found\"}"
                .as_bytes()
                .to_vec(),
        )
        .unwrap()
}
