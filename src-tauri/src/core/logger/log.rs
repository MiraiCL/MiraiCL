use crate::core::{io::file::get_file_writer, logger::level::LogLevel};
use anyhow::Result;
use rand::{rng, RngCore};
use std::{
    error::Error,
    path::PathBuf,
    sync::{Arc, Mutex, OnceLock},
    fs::create_dir_all
};
use tokio::{
    sync::mpsc,
    {
        fs::File,
        io::{AsyncWriteExt, BufWriter},
    },
};

static TX: OnceLock<Arc<Mutex<mpsc::Sender<String>>>> = OnceLock::new();
static RX: OnceLock<Arc<Mutex<mpsc::Receiver<String>>>> = OnceLock::new();

pub struct Logger {
    log_path: String,
    max_size: u64,
    enable_auto_remove: bool,
    enable_compress: bool,
    outdate_days: u32,
    log_writer: BufWriter<File>,
}

impl Logger {
    async fn create(
        log_path: String,
        max_size: u64,
        enable_auto_remove: Option<bool>,
        enable_compress: Option<bool>,
        oudate_days: Option<u32>,
    ) -> Result<Logger> {
        let mut path = PathBuf::new().join(log_path);
        for _ in 0..256 {
            // reduce duplicate
            let unchecked_path = path.join(format!(
                "{}-{}.log",
                chrono::Local::now().date_naive().format("%Y-%m-%d"),
                rng().next_u64()
            ));
            if unchecked_path.exists() {
                continue;
            }
            let parent = unchecked_path.parent().unwrap();
            if !parent.exists(){
                _= create_dir_all(parent).map_err(|e| { panic!("Unable create folder {}",e)});
            }
            path = unchecked_path;
            break;
        };
        Ok(Logger {
            log_path: format!("{}", path.display()),
            max_size: max_size,
            enable_auto_remove: match enable_auto_remove {
                Some(switch) => switch,
                None => false,
            },
            enable_compress: match enable_compress {
                Some(switch) => switch,
                None => false,
            },
            outdate_days: match oudate_days {
                Some(int) => int,
                None => 0,
            },
            log_writer: get_file_writer(path.as_path().to_str().unwrap()).await?,
        })
    }
    async fn log(&mut self, message: String) {
        println!("{}", message);
        let _ = self.log_writer.write(message.as_bytes()).await;
    }

    fn set_log_level(&mut self, level: LogLevel) {}

    async fn format(
        &mut self,
        level: LogLevel,
        module: String,
        message: String,
        e: &Option<Box<dyn Error>>,
    ) {
        let time = chrono::Local::now().time().format("%H:%M:%S%.3f");
        let level_str = match level {
            LogLevel::Trace => "Trace",
            LogLevel::Debug => "Debug",
            LogLevel::Info => "Info",
            LogLevel::Warning => "Warning",
            LogLevel::Error => "Error",
            LogLevel::Fatal => "Fatal",
        };
        self.log(match e {
            Some(e) => format!(
                "[{}] [{}] | [{}] : {}:{}",
                time, level_str, module, message, e
            ),
            None => format!("[{}] [{}] | [{}] : {}", time, level_str, module, message),
        }).await;
    }

    async fn trace(&mut self, module: String, message: String) {
        self.format(LogLevel::Error, module, message, &None).await;
    }

    async fn debug(&mut self, module: String, message: String) {
        self.format(LogLevel::Error, module, message, &None).await;
    }

    async fn info(&mut self, module: String, message: String) {
        self.format(LogLevel::Error, module, message, &None).await;
    }

    async fn warning(&mut self, e: Option<Box<dyn Error>>, module: String, message: String) {
        self.format(LogLevel::Error, module, message, &e).await;
    }

    async fn error(&mut self, e: Option<Box<dyn Error>>, module: String, message: String) {
        self.format(LogLevel::Error, module, message, &e).await;
    }

    async fn fatal(&mut self, e: Option<Box<dyn Error>>, module: String, message: String) {
        self.format(LogLevel::Fatal, module, message, &e).await;
        if e.is_some() {
            panic!("Application throw an uncxcept error {:?}", e)
        }
    }

    async fn close(&mut self){
        let _ = self.log_writer.flush().await;
    }
}

#[cfg(test)]
mod log_test{
    use super::*;

    #[tokio::test]
    async fn test(){
        let mut logger = Logger::create("./app/logs".to_string(), 65535, Some(true), Some(true), Some(0)).await.unwrap();
        logger.info("9".to_string(), "message".to_string()).await;
        logger.close().await;
    }
}