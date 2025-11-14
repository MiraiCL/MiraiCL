use tokio::process::Command;

struct Process {
    file_name: String,
    arguments: Option<Vec<String>>,
    redirect_output: Option<bool>,
    redriect_error: Option<bool>,
    redirect_input: Option<bool>,
}

impl Process {
    fn new(
        file_name: String,
        arguments: Option<Vec<String>>,
        redirect_output: Option<bool>,
        redriect_error: Option<bool>,
        redirect_input: Option<bool>,
    ) /*-> Process*/
    {
        let ro = match redirect_output {
            Some(value) => value,
            None => false,
        };
        let re = match redriect_error {
            Some(value) => value,
            None => false,
        };
        let args = match arguments {
            Some(value) => value,
            None => Vec::new(),
        };
        let ri = match redirect_input {
            Some(value) => value,
            None => false,
        };
        //Process { file_name:file_name , arguments: arguments, redirect_output: ro, redriect_error: re, redirect_input: ri }
    }

    fn start(self) {}

    fn kill(force: Option<bool>) {}

    fn get_pid() {}
}
