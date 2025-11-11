use serde::{Serialize,Deserialize};

#[derive(Serialize,Deserialize)]
pub struct CacheFile {
    etag:String,
    sha1:String
}