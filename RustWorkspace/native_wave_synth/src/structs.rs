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

#[repr(C)]
pub struct Array<T> {
    ptr: *mut T,
    length: usize,
}

impl<T> Array<T> {
    pub(crate) fn get_mutable_array(&mut self) -> &mut [T] {
        return unsafe { std::slice::from_raw_parts_mut(self.ptr, self.length) };
    }
    pub(crate) fn get_array(&self) -> &[T] {
        return unsafe { std::slice::from_raw_parts(self.ptr, self.length) };
    }
}

#[repr(C)]
pub struct WaveData {
    pub(crate) frequency: f32,
    pub(crate) amplitude: f32,
}