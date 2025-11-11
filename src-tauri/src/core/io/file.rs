use std::path::Path;
use tokio::fs::File;
use tokio::{io::{AsyncReadExt, AsyncSeekExt, BufReader,BufWriter},runtime::Runtime};
use anyhow::Result;

pub async fn get_file_limit_size(path: &str, start: u64, end: u64) -> Option<Vec<u8>> {
    let p = Path::new(path);
    if !p.exists() {
        return None;
    }

    let file = match File::open(p).await {
        Ok(f) => f,
        Err(_) => return None,
    };

    let mut file = file;
    if let Err(_) = file.seek(tokio::io::SeekFrom::Start(start)).await {
        return None; 
    }

    let mut buf = vec![0; (end - start) as usize];  
    match file.read_exact(&mut buf).await {
        Ok(_) => Some(buf),  
        Err(_) => None,
    }
}

pub async fn get_file_reader(path:&str) -> Result<BufReader<File>> {
    let p = Path::new(path);
    let file = File::open(p).await?;
    Ok(BufReader::new(file))
}

pub async fn get_file_writer(path:&str) -> Result<BufWriter<File>>{
    let p = Path::new(path);
    let file =File::open(p).await?;
    Ok(BufWriter::new(file))
}

pub async fn read_file(path:&str) -> Result<Vec<u8>>{
    let mut f = get_file_reader(path).await?;
    let mut buf:Vec<u8> = Vec::new();
    f.read_to_end(&mut buf).await?;
    Ok(buf)
}