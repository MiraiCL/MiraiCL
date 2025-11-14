use anyhow::Result;

pub struct OAuth {
    client_id: String,
    device_endpoint: String,
    authorize_endpoint: String,
    token_endpoint: String,
}
/*
impl OAuth{
    fn new(client_id:String,token_endpoint:String,device_endpoint:Option<String>,authorize_endpoint:Option<String>) -> Result<OAuth>{
        //if device_endpoint.is_none() && authorize_endpoint.is_none(){}
    }
}
*/
