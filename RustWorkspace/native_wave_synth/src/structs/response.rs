use std::ffi::CString;
use std::os::raw::c_char;

#[repr(C)]
pub struct Response<T> {
    data: T,
    error: bool,
    message: *mut c_char,
}

impl<T> Response<T> {
    pub(crate) fn ok(data: T) -> Response<T> {
        Response {
            data,
            error: false,
            message: CString::new("").unwrap().into_raw(),
        }
    }

    pub(crate) fn error(data: T, message: String) -> Response<T> {
        Response {
            data,
            error: true,
            message: CString::new(message).unwrap().into_raw(),
        }
    }
}