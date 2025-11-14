use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize)]
pub struct CacheFile {
    etag: String,
    sha1: String,
}
