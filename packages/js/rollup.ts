import type { RollupOptions } from 'rollup'
import fs from 'node:fs'
import resolve from '@rollup/plugin-node-resolve'
import { defineConfig } from 'rollup'
import esbuild from 'rollup-plugin-esbuild'

const pkg = JSON.parse(fs.readFileSync(new URL('./package.json', import.meta.url), 'utf-8'))

const year = new Date().getFullYear()

function banner() {
	return `/**
  * ${pkg.description} v${pkg.version} (${pkg.homepage})
  * Copyright 2023-${year} ${pkg.author}
  * Licensed under ${pkg.license} (https://github.com/czirok/apps/blob/main/LICENSE)
  */`
}

const inputHead = './src/Index.Head.ts'
const ecmaScriptHead = './dist/blazor.head.min.js'

const inputWebHead = './src/Index.WebHead.ts'
const ecmaScriptWebHead = './dist/blazor.web.head.min.js'

const inputBody = './src/Index.Body.ts'
const ecmaScriptBody = './dist/blazor.body.min.js'

export default defineConfig([
	{
		input: inputHead,
		output: [
			{
				format: 'iife',
				name: 'BlazorHead',
				file: ecmaScriptHead,
			},
		],
		plugins: [
			esbuild({
				sourceMap: true,
				minify: true,
				banner: banner(),
			}),
			resolve(),
		],
		external: [
			...Object.keys(pkg.dependencies || []),
		],
	},
	{
		input: inputWebHead,
		output: [
			{
				format: 'iife',
				name: 'BlazorHead',
				file: ecmaScriptWebHead,
			},
		],
		plugins: [
			esbuild({
				sourceMap: true,
				minify: true,
				banner: banner(),
			}),
			resolve(),
		],
		external: [
			...Object.keys(pkg.dependencies || []),
		],
	},
	{
		input: inputBody,
		output: [
			{
				format: 'iife',
				name: 'BlazorBody',
				file: ecmaScriptBody,
			},
		],
		plugins: [
			esbuild({
				sourceMap: true,
				minify: true,
				banner: banner(),
			}),
			resolve(),
		],
		external: [
			...Object.keys(pkg.dependencies || []),
		],
	},
] as RollupOptions[])
