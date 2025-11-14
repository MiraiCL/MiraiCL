use anyhow::Result;
use std::env::{current_dir, var};
use std::path::PathBuf;

fn get_app_data() -> Result<PathBuf> {
    let current_folder = current_dir()?;
    Ok(current_folder.join("MiraiCL").join("Application Data"))
}

fn get_process_envirment(name: &str) -> Result<String> {
    Ok(var(name)?)
}
