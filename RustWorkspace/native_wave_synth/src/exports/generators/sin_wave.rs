use std::f32::consts::PI;
use crate::structs::{Array, Response, WaveData};

#[no_mangle]
pub extern "C" fn generate_sin_wave(
    mut buffer_struct: Array<f32>,
    wave_data_struct: Array<WaveData>,
    phase_start: f32,
    num_channels: i32,
    sample_rate: i32,
) -> Response<f32> {
    let channels = num_channels as usize;
    let buffer = buffer_struct.get_mutable_array();
    let wave_data = wave_data_struct.get_array();

    if buffer.len() % channels != 0 {
        return Response::error(
            phase_start,
            format!("'buffer' length must be a multiple of 'num_channels'.\n\
            Buffer length: {}, num_channels: {}", buffer.len(), num_channels));
    }

    let target_length = buffer.len() / channels;
    if wave_data.len() != target_length {
        return Response::error(
            phase_start,
            format!("'wave_data' must be the length of one buffer channel.\n\
        Target length: {}, Actual length: {}", target_length, wave_data.len()));
    }

    let mut phase = phase_start;
    for i in (0..buffer.len()).step_by(num_channels as usize) {
        let wdi = i / num_channels as usize;
        let phase_advance = wave_data[wdi].frequency / sample_rate as f32;
        phase = (phase + phase_advance) % (2f32 * PI);

        let val = phase.sin() as f32 * wave_data[wdi].amplitude;
        for bi in 0..num_channels as usize { buffer[i + bi] = val }
    }

    return Response::ok(phase);
}