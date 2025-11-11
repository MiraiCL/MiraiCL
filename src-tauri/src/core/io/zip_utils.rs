use tokio::{io::BufReader,fs::File};
use zip::ZipArchive;
use crate::core::io::file::{get_file_reader,get_file_writer};
use anyhow::Result;

pub struct ZipFile{
    innerReader:ZipArchive<BufReader<File>>
}

impl ZipFile{
    async fn new(path:&str) -> Result<ZipFile>{
        ZipFile{
            innerReader: ZipArchive::new(get_file_reader(path)?.)
        }
    }
    fn extract(entry_name:&str,path:&str){

    }
    fn extract_all(path:&str){

    }
    fn compress(){
        
    }

    fn get_entries(){

    }
    fn get_entry(name:&str){

    }
    fn create_entry(name:&str){

    }
}