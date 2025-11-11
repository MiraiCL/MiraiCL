use tokio::io::BufWriter;
use std::error::Error;
use std::sync::{mpsc, Arc, Mutex, OnceLock};
use crate::core::logger::level::LogLevel;


static TX: OnceLock<Arc<Mutex<mpsc::Sender<String>>>> = OnceLock::new();
static RX: OnceLock<Arc<Mutex<mpsc::Receiver<String>>>> = OnceLock::new();

struct Logger {
    log_path: String,
    max_size: u64,
    enable_auto_remove: bool,
    enable_compress: bool,
    outdate_days: u32,
    log_writer:
}

impl Logger {
    fn new(
        log_path: String,
        max_size: u64,
        enable_auto_remove: Option<bool>,
        enable_compress: Option<bool>,
        oudate_days: Option<u32>,
    ) -> Logger {
        Logger {
            log_path: log_path,
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
        }
    }
    fn log(message: &str) {

    }

    fn set_log_level() {

    }

    fn log_with_level(level:LogLevel,module:String,message:String) {

    }

    fn trace() {}

    fn debug() {}

    fn info() {}

    fn warning() {}

    fn error() {}

    fn fatal(e: Box<dyn Error>, module: &str, message: &str) {
        panic!("Application throw an uncxcept error {}", e)
    }
}
