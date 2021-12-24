use std::f32::consts::PI;

#[repr(C)]
pub struct wave_data {
    frequency: f32,
    amplitude: f32,
}

#[repr(C)]
pub struct array<T> {
    ptr: *mut T,
    length: usize,
}

impl<T> array<T> {

    // creates a mutable array from the pointer data
    fn get_mutable_array(&mut self) -> &mut [T] {
        return unsafe { std::slice::from_raw_parts_mut(self.ptr, self.length) };
    }

    // creates a read only array from the pointer data
    fn get_array(&self) -> &[T] {
        return unsafe { std::slice::from_raw_parts(self.ptr, self.length) };
    }
}

#[no_mangle]
pub extern "C" fn merge_audio_buffers(mut main_buffer: array<f32>, add_buffer: array<f32>) {
    let buffer = main_buffer.get_mutable_array();
    let add = add_buffer.get_array();
    let combine_length = buffer.len().min(add.len());

    for i in 0..combine_length {
        buffer[i] = (buffer[i] + add[i]).min(1f32).max(-1f32);
    }
}

#[no_mangle]
pub extern "C" fn generate_sin_wave(
    mut buffer_struct: array<f32>,
    wave_data_struct: array<wave_data>,
    phase_start: f32,
    sample_rate: i32,
) -> f32 {
    let buffer = buffer_struct.get_mutable_array();
    let wave_data = wave_data_struct.get_array();

    // return early if arrays are not of equal length
    if wave_data.len() != buffer.len() {
        return phase_start;
    }

    let mut phase = phase_start;
    for i in (0..buffer.len()).step_by(2) {
        let phase_advance = wave_data[i].frequency / sample_rate as f32;
        phase = (phase + phase_advance) % (2f32 * PI);

        buffer[i + 0] = phase.sin() as f32 * wave_data[i].amplitude;
        buffer[i + 1] = phase.sin() as f32 * wave_data[i].amplitude;
    }

    return phase;
}