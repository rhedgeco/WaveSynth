use crate::structs::{Array, Response};

#[no_mangle]
pub extern "C" fn merge_audio_buffers(
    mut main_buffer: Array<f32>,
    add_buffer: Array<f32>,
) -> Response<i32> {
    let buffer = main_buffer.get_mutable_array();
    let add = add_buffer.get_array();
    let combine_length = buffer.len().min(add.len());

    for i in 0..combine_length {
        buffer[i] = (buffer[i] + add[i]).min(1f32).max(-1f32);
    }

    return Response::ok(0);
}