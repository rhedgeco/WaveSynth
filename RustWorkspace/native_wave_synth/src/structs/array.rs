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