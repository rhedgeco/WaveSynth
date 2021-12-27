use std::f32::consts::PI;
use crate::structs::{Array, Response, WaveData};

fn lerp(v1: f32, v2: f32, t: f32) -> f32 {
    return ((v2 - v1) * t) + v1;
}

#[no_mangle]
pub extern "C" fn generate_cache_wave(
    mut buffer_struct: Array<f32>,
    wave_data_struct: Array<WaveData>,
    cache_struct: Array<f32>,
    phase_start: f32,
    num_channels: i32,
    sample_rate: i32,
) -> Response<f32> {
    let channels = num_channels as usize;
    let buffer = buffer_struct.get_mutable_array();
    let wave_data = wave_data_struct.get_array();

    let cache = cache_struct.get_array();
    let cache_len = cache.len() as f32 - 1f32;

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

        let phase_adjust = phase / (2f32 * PI);
        let bin_index: usize = (phase_adjust * cache_len).trunc() as usize;
        let bin1 = cache[bin_index + 0];
        let bin2 = cache[bin_index + 1];
        let bin_lerp = lerp(bin1, bin2, (phase_adjust * cache_len) % 1.0);

        let val = bin_lerp * wave_data[wdi].amplitude;
        for bi in 0..num_channels as usize { buffer[i + bi] = val }
    }

    return Response::ok(phase);
}