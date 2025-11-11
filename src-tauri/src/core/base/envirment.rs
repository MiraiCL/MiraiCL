use std::env::{var,current_dir};
use std::path::PathBuf;
use anyhow::Result;

fn get_app_data() -> Result<PathBuf>{
    let current_folder = current_dir()?;
    Ok(current_folder.join("MiraiCL").join("Application Data"))
}

fn get_process_envirment(name:&str) -> Result<String>{
    Ok(var(name)?)
}